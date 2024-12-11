<script lang="ts">
  const { bids } = $props();
</script>

{#each bids as bid}
  <li>
    <p>{bid.amount}</p>
  </li>
{/each}
