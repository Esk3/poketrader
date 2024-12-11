<script lang="ts">
  const { listing }: { listing: Listing } = $props();
  interface Listing {
    listingId: number;
    pokemonUserId: number;
    inventoryId: number;
    createTimestamp: string;
    closedTimestamp: string;
    cancled: boolean;
  }
</script>

<div class="listing">
  <a href="/market/{listing.listingId}">
    <p>TODO: InventoryItemCard {listing.inventoryId}</p>
    <p>Created at: {listing.createTimestamp}</p>
    <p>Max bid value: todo</p>
  </a>
</div>
