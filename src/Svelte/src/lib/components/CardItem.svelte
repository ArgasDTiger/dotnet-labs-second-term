<script lang="ts" generics="T">
    import type { Snippet } from 'svelte';

    interface Props {
        item?: T;
        headerContent?: Snippet;
        middleContent?: Snippet;
        listContent?: Snippet;
        showActions?: boolean;
        showDetails?: boolean;
        showEdit?: boolean;
        showDelete?: boolean;
        onDetailsClick?: (item: T) => void;
        onEditClick?: (item: T) => void;
        onDeleteClick?: (item: T) => void;
    }

    let {
        item,
        headerContent,
        middleContent,
        listContent,
        showActions = true,
        showDetails = false,
        showEdit = true,
        showDelete = true,
        onDetailsClick,
        onEditClick,
        onDeleteClick
    }: Props = $props();

    function handleDetailsClick() {
        if (item && onDetailsClick) {
            onDetailsClick(item);
        }
    }

    function handleEditClick() {
        if (item && onEditClick) {
            onEditClick(item);
        }
    }

    function handleDeleteClick() {
        if (item && onDeleteClick) {
            onDeleteClick(item);
        }
    }
</script>

<div class="card">
    {#if headerContent}
        <div class="title">
            {@render headerContent()}
        </div>
    {/if}

    {#if middleContent}
        <div class="description">
            {@render middleContent()}
        </div>
    {/if}

    {#if listContent}
        <ul class="subject-data">
            {@render listContent()}
        </ul>
    {/if}

    {#if showActions}
        <div class="actions-container">
            {#if showDetails}
                <button
                        class="btn-primary"
                        type="button"
                        onclick={handleDetailsClick}
                >
                    Details
                </button>
            {/if}

            {#if showEdit}
                <button
                        class="btn-secondary"
                        type="button"
                        onclick={handleEditClick}
                >
                    Edit
                </button>
            {/if}

            {#if showDelete}
                <button
                        class="btn-danger"
                        type="button"
                        onclick={handleDeleteClick}
                >
                    Delete
                </button>
            {/if}
        </div>
    {/if}
</div>

<style lang="scss">
  .card {
    width: 20%;
    background-color: #404343;
    margin-right: 2em;
    margin-bottom: 1em;
    padding: 0.4em;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }

  .title {
    text-align: center;
    font-size: 1.2em;
    font-weight: bold;
    color: var(--text-primary);
  }

  .description {
    color: #afafaf;
    text-align: left;
    font-size: 1em;
  }

  .subject-data {
    padding-left: 0;
    margin-top: 0.7em;
    color: #e1dbdb;

    .subject-item {
      list-style-type: none;
      padding-left: 0;
      display: flex;

      .header {
        font-weight: bold;
      }

      .value {
        margin-left: 0.5em;
      }
    }
  }

  .actions-container {
    width: 100%;
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: 0.5em;

    button {
      padding: 0.2em 1em;
    }
  }
</style>