<script lang="ts">
  import { ApiUrl } from "$lib";
  import PokemonSprite from "$lib/components/PokemonSprite.svelte";
  import { onMount } from "svelte";
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

  let maxBid = $state(null);
  onMount(async () => {
    const res = await fetch(
      ApiUrl + "Market/" + listing.listingId + "/bids/info/max",
    );
    const data = await res.json();
    maxBid = data.amount;
  });
</script>

<div class="listing">
  <a href="/market/{listing.listingId}">
    {listing.pokemonName}
    <PokemonSprite url={listing.spriteUrl} name={listing.pokemonName} />
    <p>Created at: {listing.createTimestamp}</p>
    <p>Max bid value: {maxBid}</p>
  </a>
</div>
