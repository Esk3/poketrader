<script lang="ts">
  import ItemView from "$lib/components/ItemView.svelte";
  const { data } = $props();
  console.log(data);
</script>

<a href="/market/create">Lag ny handel</a>

<ul>
  {#each data.listings as fut}
    <li>
      {#await fut then listing}
        <a href="/market/{listing.id}">
          <p>Creator: {listing.username}</p>
          <ItemView item={listing.item} />
          <p>Max bid: <span class="money">{listing.maxBidValue}</span></p>
        </a>
      {/await}
    </li>
  {/each}
</ul>

<style>
  ul {
    padding: 0;
    list-style: none;
    display: flex;
    flex-direction: column;
    gap: 1em;
  }
  li {
    border: 1px solid black;
    width: 200px;
    & * {
      color: var(--text);
      text-decoration: none;
    }
  }
</style>
