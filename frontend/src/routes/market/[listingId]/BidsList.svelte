<script lang="ts">
  const { bids } = $props();
</script>

{#each bids as bid}
  <li>
    <p>User: {bid.pokemonUserId}</p>
    <p>amount: {bid.amount}</p>
  </li>
{/each}
