<script lang="ts">
    import type { Snippet } from 'svelte';

    interface Props {
        isOpen: boolean;
        title: string;
        onClose: () => void;
        children: Snippet;
        actions?: Snippet;
    }

    let { 
        isOpen = $bindable(false), 
        title,
        onClose,
        children, 
        actions 
    }: Props = $props();

    function handleBackdropClick(event: MouseEvent) {
        if (event.target === event.currentTarget) {
            onClose();
        }
    }

    function handleKeydown(event: KeyboardEvent) {
        if (event.key === 'Escape' && isOpen) {
            onClose();
        }
    }
</script>

<svelte:window onkeydown={handleKeydown} />

{#if isOpen}
    <div class="modal-backdrop" onclick={handleBackdropClick}>
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">{title}</h5>
                    <button type="button" class="btn-close" onclick={onClose}>×</button>
                </div>
                <div class="modal-body">
                    {@render children()}
                </div>
                {#if actions}
                    <div class="modal-footer">
                        {@render actions()}
                    </div>
                {/if}
            </div>
        </div>
    </div>
{/if}

<style lang="scss">
  .modal-backdrop {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
  }

  .modal-dialog {
    width: 90%;
    max-width: 500px;
    max-height: 90vh;
    overflow-y: auto;
  }

  .modal-content {
    background-color: #2c2f33;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);
  }

  .modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
    border-bottom: 1px solid #444;
  }

  .modal-title {
    margin: 0;
    font-size: 1.25rem;
    color: #fff;
  }

  .btn-close {
    background: none;
    border: none;
    color: #fff;
    font-size: 1.5rem;
    cursor: pointer;
    padding: 0;
    width: 2rem;
    height: 2rem;
    display: flex;
    align-items: center;
    justify-content: center;

    &:hover {
      color: #ccc;
    }
  }

  .modal-body {
    padding: 1rem;
    color: #fff;
  }

  .modal-footer {
    display: flex;
    justify-content: flex-end;
    gap: 0.5rem;
    padding: 1rem;
    border-top: 1px solid #444;
  }
</style>