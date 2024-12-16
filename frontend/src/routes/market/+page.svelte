<script lang="ts">
  const listings = [{ id: 1, name: "pokemon name" }];
  import ListingCardList from "./ListingCardList.svelte";
  const { data } = $props();
  console.log(data);
</script>

<a href="/market/create">Lag ny handel</a>
<ListingCardList listings={data.listings} />
