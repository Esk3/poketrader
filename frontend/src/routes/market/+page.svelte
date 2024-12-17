<script lang="ts">
  import ItemView from "$lib/components/ItemView.svelte";
  import ListingCardList from "./ListingCardList.svelte";
  const { data } = $props();
  console.log(data);
</script>

<a href="/market/create">Lag ny handel</a>
{#each data.listings as fut}
  {#await fut then listing}
    <a href="/market/{listing.id}">
      <p>Creator: {listing.username}</p>
      <ItemView item={listing.item} />
      <p>Max bid: {listing.maxBidValue}</p>
    </a>
  {/await}
{/each}
