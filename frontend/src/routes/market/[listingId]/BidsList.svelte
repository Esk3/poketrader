<script lang="ts">
  import Bid from "./Bid.svelte";
  const { bids } = $props();
</script>

{#each bids as bid}
  <li>
    <Bid {bid} />
  </li>
{/each}
