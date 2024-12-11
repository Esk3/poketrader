<script lang="ts">
  import PokemonSprite from "$lib/components/PokemonSprite.svelte";
  import ListingCardList from "./ListingCardList.svelte";

  const { listing }: { listing: Listing } = $props();
  interface Listing {
    listingId: number;
    pokemonUserId: number;
    inventoryId: number;
    pokemonId: number;
    spriteUrl: string;
    pokemonName: string;
    createTimestamp: string;
    closedTimestamp: string;
    cancled: boolean;
  }
</script>

<div class="listing">
  <a href="/market/{listing.listingId}">
    {listing.pokemonName}
    <PokemonSprite url={listing.spriteUrl} name={listing.pokemonName} />
    <p>Created at: {listing.createTimestamp}</p>
    <p>Max bid value: todo</p>
  </a>
</div>
