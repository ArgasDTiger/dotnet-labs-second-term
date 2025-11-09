<script lang="ts">
    import {onMount} from 'svelte';
    import {goto} from '$app/navigation';
    import CardItem from '$lib/components/CardItem.svelte';
    import Modal from '$lib/components/Modal.svelte';
    import {clientsService, type CreateClientRequest, type UpdateClientRequest} from '$lib/services/clients.service.ts';
    import type {Client} from '$lib/types/client.model';

    let clients = $state<Client[]>([]);
    let selectedClient = $state<Client | null>(null);
    let isNewClient = $state(false);
    let showEditModal = $state(false);
    let showDeleteModal = $state(false);

    onMount(async () => {
        await loadClients();
    });

    async function loadClients() {
        const result = await clientsService.getClients();

        if (result.success) {
            clients = result.data;
        } else {
            console.error(result.error.message);
        }
    }

    function openNewClientModal() {
        selectedClient = {
            id: '',
            firstName: '',
            middleName: '',
            lastName: '',
            phoneNumber: '',
            homeAddress: '',
            passportSeries: null,
            passportNumber: '',
            rentedMovies: []
        };
        isNewClient = true;
        showEditModal = true;
    }

    function openEditModal(client: Client) {
        selectedClient = {...client};
        isNewClient = false;
        showEditModal = true;
    }

    function closeEditModal() {
        showEditModal = false;
        selectedClient = null;
        isNewClient = false;
    }

    async function handleClientSaved() {
        if (!selectedClient) return;

        if (isNewClient) {
            const request: CreateClientRequest = {
                firstName: selectedClient.firstName,
                middleName: selectedClient.middleName,
                lastName: selectedClient.lastName,
                phoneNumber: selectedClient.phoneNumber,
                homeAddress: selectedClient.homeAddress,
                passportSeries: selectedClient.passportSeries,
                passportNumber: selectedClient.passportNumber
            };

            const result = await clientsService.createClient(request);

            if (result.success) {
                await loadClients();
            } else {
                console.error(result.error.message);
            }
        } else {
            const request: UpdateClientRequest = {
                firstName: selectedClient.firstName,
                middleName: selectedClient.middleName,
                lastName: selectedClient.lastName,
                phoneNumber: selectedClient.phoneNumber,
                homeAddress: selectedClient.homeAddress,
                passportSeries: selectedClient.passportSeries,
                passportNumber: selectedClient.passportNumber
            };

            const result = await clientsService.updateClient(selectedClient.id, request);

            if (result.success) {
                await loadClients();
            } else {
                console.error(result.error.message);
            }
        }

        closeEditModal();
    }

    function openDeleteModal(client: Client) {
        selectedClient = client;
        showDeleteModal = true;
    }

    function closeDeleteModal() {
        showDeleteModal = false;
        selectedClient = null;
    }

    async function handleDeleteConfirmed() {
        if (!selectedClient) return;

        const result = await clientsService.deleteClient(selectedClient.id);

        if (result.success) {
            clients = clients.filter(c => c.id !== selectedClient!.id);
        } else {
            console.error(result.error.message);
        }

        closeDeleteModal();
    }

    function onDetailsClick(client: Client) {
        goto(`/clients/${client.id}`);
    }
</script>

<h3>Clients</h3>

<div class="card-container">
    {#each clients as client (client.id)}
        <CardItem
                item={client}
                showDetails={true}
                showEdit={true}
                showDelete={true}
                onDetailsClick={onDetailsClick}
                onEditClick={openEditModal}
                onDeleteClick={openDeleteModal}
        >
            {#snippet headerContent()}
                {client.lastName} {client.firstName} {client.middleName}
            {/snippet}

            {#snippet middleContent()}
                {client.phoneNumber} <br/> {client.homeAddress}
            {/snippet}

            {#snippet listContent()}
                {#if client.passportSeries}
                    <li class="subject-item">
                        <div class="header">Passport Series:</div>
                        <div class="value">{client.passportSeries}</div>
                    </li>
                {/if}
                <li class="subject-item">
                    <div class="header">Passport Number:</div>
                    <div class="value">{client.passportNumber}</div>
                </li>
            {/snippet}
        </CardItem>
    {/each}

    <button class="btn-black btn-add w-100" onclick={openNewClientModal}>
        Add New Client
    </button>
</div>

<Modal
        bind:isOpen={showEditModal}
        title={isNewClient ? 'Add New Client' : 'Edit Client'}
        onClose={closeEditModal}
>
    {#snippet children()}
        {#if selectedClient}
            <div class="mb-3">
                <label for="firstName" class="form-label">First Name</label>
                <input id="firstName" bind:value={selectedClient.firstName} class="form-control"/>
            </div>

            <div class="mb-3">
                <label for="middleName" class="form-label">Middle Name</label>
                <input id="middleName" bind:value={selectedClient.middleName} class="form-control"/>
            </div>

            <div class="mb-3">
                <label for="lastName" class="form-label">Last Name</label>
                <input id="lastName" bind:value={selectedClient.lastName} class="form-control"/>
            </div>

            <div class="mb-3">
                <label for="phoneNumber" class="form-label">Phone Number</label>
                <input id="phoneNumber" bind:value={selectedClient.phoneNumber} class="form-control"/>
            </div>

            <div class="mb-3">
                <label for="homeAddress" class="form-label">Home Address</label>
                <textarea id="homeAddress" bind:value={selectedClient.homeAddress} class="form-control"></textarea>
            </div>

            <div class="mb-3">
                <label for="passportSeries" class="form-label">Passport Series</label>
                <input id="passportSeries" bind:value={selectedClient.passportSeries} class="form-control"/>
            </div>

            <div class="mb-3">
                <label for="passportNumber" class="form-label">Passport Number</label>
                <input id="passportNumber" bind:value={selectedClient.passportNumber} class="form-control"/>
            </div>
        {/if}
    {/snippet}

    {#snippet actions()}
        <button class="btn btn-primary" onclick={handleClientSaved}>Save</button>
        <button class="btn btn-secondary" onclick={closeEditModal}>Cancel</button>
    {/snippet}
</Modal>

<Modal
        bind:isOpen={showDeleteModal}
        title="Confirm Delete"
        onClose={closeDeleteModal}
>
    {#snippet children()}
        {#if selectedClient}
            <p>
                Are you sure you want to delete <strong>{selectedClient.firstName} {selectedClient.lastName}</strong>?
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