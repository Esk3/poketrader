<script lang="ts">
  const listings = [{ id: 1, name: "pokemon name" }];
  import ListingCardList from "./ListingCardList.svelte";
  const { data } = $props();
</script>

<ListingCardList listings={data.listings} />
