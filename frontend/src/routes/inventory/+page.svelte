<script lang="ts">
  import FlipCard from "$lib/components/FlipCard.svelte";
  import ItemView from "$lib/components/ItemView.svelte";
  import PokemonCardLoading from "$lib/components/PokemonCardLoading.svelte";

  const { data } = $props();
</script>

<ul>
  {#each data.inventory as fut, i}
    <li>
      {#await fut}
        <PokemonCardLoading />
      {:then item}
        <FlipCard
          width="130px"
          height="200px"
          delay={i * 100}
          loading={PokemonCardLoading}
        >
          <ItemView {item} />
        </FlipCard>
      {/await}
    </li>
  {:else}
    ...du eier ikke noen pokemon kort ennå. kjøp noen i <a href="/shop"
      >buttiken</a
    >
  {/each}
</ul>

<style>
  ul {
    list-style: none;
    padding: 0;
    display: flex;
    gap: 1em;
  }
</style>
