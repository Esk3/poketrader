<script lang="ts">
  import ListingCard from "./ListingCard.svelte";

  const { listings } = $props();
</script>

{#each listings as listing}
  <ListingCard {listing} />
{/each}
