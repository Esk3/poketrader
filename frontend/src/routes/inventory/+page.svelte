<script lang="ts">
  import ItemView from "$lib/components/ItemView.svelte";

  const { data } = $props();
</script>

{#each data.inventory as fut}
  {#await fut then item}
    <ItemView {item} />
  {/await}
{/each}
