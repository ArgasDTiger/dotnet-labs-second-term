<script lang="ts">
    import {onMount} from 'svelte';
    import {page} from '$app/state';
    import {goto} from '$app/navigation';
    import CardItem from '$lib/components/CardItem.svelte';
    import Modal from '$lib/components/Modal.svelte';
    import {clientsService} from '$lib/services/clients.service.ts';
    import type {Client, ClientMovie} from '$lib/types/client.model';
    import {moviesService} from "$lib/services/movies.service.ts";

    const clientId = $derived(page.params.id);
    let client = $state<Client | null>(null);
    let selectedMovie = $state<ClientMovie | null>(null);
    let showReturnModal = $state(false);

    onMount(async () => {
        await loadClient();
    });

    async function loadClient() {
        const result = await clientsService.getClientById(clientId.toString());

        if (result.success) {
            client = result.data;
        } else {
            console.error(result.error.message);
        }
    }

    function openReturnModal(movie: ClientMovie) {
        selectedMovie = movie;
        showReturnModal = true;
    }

    function closeReturnModal() {
        showReturnModal = false;
        selectedMovie = null;
    }

    async function handleReturnConfirmed() {
        if (!selectedMovie || !client) return;

        const result = await moviesService.returnMovie(selectedMovie.movieId, client.id);
        if (result.success) {
            selectedMovie = null;
        } else {
            console.error(result.error.message);
        }
        await loadClient();

        closeReturnModal();
    }

    function goBack() {
        goto('/clients');
    }

    function formatDate(date: string): string {
        return new Date(date).toISOString().split('T')[0];
    }

    function formatCurrency(amount: number): string {
        return new Intl.NumberFormat('en-US', {style: 'currency', currency: 'USD'}).format(amount);
    }
</script>

{#if client}
    <div class="container">
        <div class="row mb-4">
            <div class="col">
                <button onclick={goBack} class="btn btn-secondary">← Back to Clients</button>
            </div>
        </div>

        <div class="card-info mb-4">
            <div class="card-body">
                <h1 class="card-title">{client.lastName} {client.firstName} {client.middleName}</h1>
                <hr/>

                <div class="row">
                    <div class="col-md-6">
                        <p><strong>Phone Number:</strong> {client.phoneNumber}</p>
                        <p><strong>Home Address:</strong> {client.homeAddress}</p>
                    </div>
                    <div class="col-md-6">
                        {#if client.passportSeries}
                            <p><strong>Passport Series:</strong> {client.passportSeries}</p>
                        {/if}
                        <p><strong>Passport Number:</strong> {client.passportNumber}</p>
                    </div>
                </div>
            </div>
        </div>

        {#if client.rentedMovies && client.rentedMovies.length > 0}
            <h2>Rented Movies</h2>
            <div class="card-container">
                {#each client.rentedMovies as movie, index (`${movie.movieId}-${index}`)}
                    <CardItem
                            item={movie}
                            showDetails={false}
                            showEdit={false}
                            showDelete={false}
                            showActions={false}
                    >
                        {#snippet headerContent()}
                            {movie.movieTitle}
                        {/snippet}

                        {#snippet listContent()}
                            <li class="subject-item">
                                <div class="header">Expected return date:</div>
                                <div class="value">{formatDate(movie.expectedReturnDate)}</div>
                            </li>
                            <li class="subject-item">
                                <div class="header">Price per day:</div>
                                <div class="value">{formatCurrency(movie.pricePerDay)}</div>
                            </li>
                            {#if movie.returnedDate}
                                <li class="subject-item">
                                    <div class="header">Returned date:</div>
                                    <div class="value">{formatDate(movie.returnedDate)}</div>
                                </li>
                            {/if}
                            {#if !movie.returnedDate}
                                <li>
                                    <button
                                            type="button"
                                            class="btn btn-primary"
                                            onclick={() => openReturnModal(movie)}
                                    >
                                        Return
                                    </button>
                                </li>
                            {/if}
                        {/snippet}
                    </CardItem>
                {/each}
            </div>
        {:else}
            <div class="alert alert-info">
                This client has no rented movies.
            </div>
        {/if}
    </div>
{:else}
    <p>Loading...</p>
{/if}

<Modal
        bind:isOpen={showReturnModal}
        title="Confirm Return"
        onClose={closeReturnModal}
>
    {#snippet children()}
        {#if selectedMovie}
            <p>Are you sure you want to return <strong>{selectedMovie.movieTitle}</strong>?</p>
        {/if}
    {/snippet}

    {#snippet actions()}
        <button class="btn btn-primary" onclick={handleReturnConfirmed}>Return Movie</button>
        <button class="btn btn-secondary" onclick={closeReturnModal}>Cancel</button>
    {/snippet}
</Modal>

<style lang="scss">
  .container {
    padding: 1rem;
  }

  .mb-4 {
    margin-bottom: 1.5rem;
  }

  .card-info {
    background-color: #404343;
    border-radius: 8px;
    padding: 1.5rem;
  }

  .card-body {
    color: #fff;
  }

  .card-title {
    margin: 0 0 1rem 0;
    font-size: 1.75rem;
    color: #fff;
  }

  hr {
    border: none;
    border-top: 1px solid #555;
    margin: 1rem 0;
  }

  .row {
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
  }

  .col-md-6 {
    flex: 1;
    min-width: 250px;
  }

  p {
    margin: 0.5rem 0;
  }

  h2 {
    margin-bottom: 1rem;
  }

  .card-container {
    display: flex;
    flex-wrap: wrap;
    gap: 1em;
  }

  .alert {
    padding: 1rem;
    border-radius: 4px;
    margin-bottom: 1rem;
  }

  .alert-info {
    background-color: #3a5f7d;
    color: #fff;
    border: 1px solid #4a7fa0;
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
</style>