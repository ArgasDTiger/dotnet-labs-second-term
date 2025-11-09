<script lang="ts">
    import {onMount} from 'svelte';
    import CardItem from '$lib/components/CardItem.svelte';
    import Modal from '$lib/components/Modal.svelte';
    import {moviesService, type CreateMovieRequest, type UpdateMovieRequest} from '$lib/services/movies.service.ts';
    import type {Movie} from '$lib/types/movie.model';

    let movies = $state<Movie[]>([]);
    let selectedMovie = $state<Movie | null>(null);
    let isNewMovie = $state(false);
    let showEditModal = $state(false);
    let showDeleteModal = $state(false);

    onMount(async () => {
        await loadMovies();
    });

    async function loadMovies() {
        const result = await moviesService.getMovies();

        if (result.success) {
            movies = result.data;
        } else {
            console.error(result.error.message);
        }
    }

    function openNewMovieModal() {
        selectedMovie = {
            id: '',
            title: '',
            description: '',
            pricePerDay: 0,
            collateralValue: 0
        };
        isNewMovie = true;
        showEditModal = true;
    }

    function openEditModal(movie: Movie) {
        selectedMovie = {...movie};
        isNewMovie = false;
        showEditModal = true;
    }

    function closeEditModal() {
        showEditModal = false;
        selectedMovie = null;
        isNewMovie = false;
    }

    async function handleMovieSaved() {
        if (!selectedMovie) return;

        if (isNewMovie) {
            const request: CreateMovieRequest = {
                title: selectedMovie.title,
                description: selectedMovie.description,
                pricePerDay: selectedMovie.pricePerDay,
                collateralValue: selectedMovie.collateralValue
            };

            const result = await moviesService.createMovie(request);

            if (result.success) {
                await loadMovies();
            } else {
                console.error(result.error.message);
            }
        } else {
            const request: UpdateMovieRequest = {
                title: selectedMovie.title,
                description: selectedMovie.description,
                pricePerDay: selectedMovie.pricePerDay,
                collateralValue: selectedMovie.collateralValue
            };

            const result = await moviesService.updateMovie(selectedMovie.id, request);

            if (result.success) {
                await loadMovies();
            } else {
                console.error(result.error.message);
            }
        }

        closeEditModal();
    }

    function openDeleteModal(movie: Movie) {
        selectedMovie = movie;
        showDeleteModal = true;
    }

    function closeDeleteModal() {
        showDeleteModal = false;
        selectedMovie = null;
    }

    async function handleDeleteConfirmed() {
        if (!selectedMovie) return;

        const result = await moviesService.deleteMovie(selectedMovie.id);

        if (result.success) {
            movies = movies.filter(m => m.id !== selectedMovie!.id);
        } else {
            console.error(result.error.message);
        }

        closeDeleteModal();
    }
</script>

<h3>Movies</h3>

<div class="card-container">
    {#each movies as movie (movie.id)}
        <CardItem
                item={movie}
                showDetails={false}
                showEdit={true}
                showDelete={true}
                onEditClick={openEditModal}
                onDeleteClick={openDeleteModal}
        >
            {#snippet headerContent()}
                {movie.title}
            {/snippet}

            {#snippet middleContent()}
                {movie.description}
            {/snippet}

            {#snippet listContent()}
                <li class="subject-item">
                    <div class="header">Price per day:</div>
                    <div class="value">{movie.pricePerDay}</div>
                </li>
                <li class="subject-item">
                    <div class="header">Collateral value:</div>
                    <div class="value">{movie.collateralValue}</div>
                </li>
            {/snippet}
        </CardItem>
    {/each}

    <button class="btn-black btn-add w-100" onclick={openNewMovieModal}>
        Add New Movie
    </button>
</div>

<Modal
        bind:isOpen={showEditModal}
        title={isNewMovie ? 'Add New Movie' : 'Edit Movie'}
        onClose={closeEditModal}
>
    {#snippet children()}
        {#if selectedMovie}
            <div class="mb-3">
                <label for="title" class="form-label">Title</label>
                <input id="title" bind:value={selectedMovie.title} class="form-control"/>
            </div>

            <div class="mb-3">
                <label for="description" class="form-label">Description</label>
                <textarea id="description" bind:value={selectedMovie.description} class="form-control"></textarea>
            </div>

            <div class="mb-3">
                <label for="pricePerDay" class="form-label">Price per day</label>
                <input id="pricePerDay" bind:value={selectedMovie.pricePerDay} type="number" class="form-control"/>
            </div>

            <div class="mb-3">
                <label for="collateralValue" class="form-label">Collateral value</label>
                <input id="collateralValue" bind:value={selectedMovie.collateralValue} type="number"
                       class="form-control"/>
            </div>
        {/if}
    {/snippet}

    {#snippet actions()}
        <button class="btn btn-primary" onclick={handleMovieSaved}>Save</button>
        <button class="btn btn-secondary" onclick={closeEditModal}>Cancel</button>
    {/snippet}
</Modal>

<Modal
        bind:isOpen={showDeleteModal}
        title="Confirm Delete"
        onClose={closeDeleteModal}
>
    {#snippet children()}
        {#if selectedMovie}
            <p>
                Are you sure you want to delete <strong>{selectedMovie.title}</strong>?
            </p>
        {/if}
    {/snippet}

    {#snippet actions()}
        <button class="btn btn-danger" onclick={handleDeleteConfirmed}>Delete</button>
        <button class="btn btn-secondary" onclick={closeDeleteModal}>Cancel</button>
    {/snippet}
</Modal>

<style lang="scss">
  .card-container {
    display: flex;
    flex-wrap: wrap;
    gap: 1em;
  }

  .mb-3 {
    margin-bottom: 1rem;
  }

  .form-label {
    display: block;
    margin-bottom: 0.5rem;
    font-weight: 500;
  }

  .form-control {
    width: 100%;
    padding: 0.5rem;
    background-color: #3a3d41;
    border: 1px solid #555;
    border-radius: 4px;
    color: #fff;
    font-size: 1rem;

    &:focus {
      outline: none;
      border-color: #4a9eff;
    }
  }

  textarea.form-control {
    min-height: 100px;
    resize: vertical;
  }

  .btn {
    padding: 0.5rem 1rem;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-size: 1rem;
  }

  .btn-primary {
    background-color: #4a9eff;
    color: #fff;

    &:hover {
      background-color: #3a8eef;
    }
  }

  .btn-secondary {
    background-color: #6c757d;
    color: #fff;

    &:hover {
      background-color: #5c636a;
    }
  }

  .btn-danger {
    background-color: #dc3545;
    color: #fff;

    &:hover {
      background-color: #c82333;
    }
  }

  .btn-black {
    background-color: #333;
    color: #fff;

    &:hover {
      background-color: #444;
    }
  }

  .w-100 {
    width: 100%;
  }

  .btn-add {
    padding: 1rem;
    font-size: 1.1rem;
  }
</style>