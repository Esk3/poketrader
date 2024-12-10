<script lang="ts">
  const { data } = $props();
  console.log(data.inventory);
</script>

show cards with maybe view for doublecates and one view just showing total
number

{#each data.inventory as item}
  <div>
    {item.inventoryId}
  </div>
{/each}
