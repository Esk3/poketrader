<script>
  import PokemonCardLoading from "$lib/components/PokemonCardLoading.svelte";
  import FlipCard from "$lib/components/FlipCard.svelte";
  import PokemonPreView from "$lib/components/PokemonPreView.svelte";
  const { children, data } = $props();
</script>

<div>

<ul>
  {#each data.pokemon as item, i}
    <li>
      <a href="/shop/{item.pokemonId}">
        {#await item.pokemon}
          <PokemonCardLoading/>
        {:then pokemon}
          <FlipCard width="130px" height="200px" delay={i*100} loading={PokemonCardLoading}>
          <PokemonPreView {pokemon} >
          <span class="money">{item.cost} </span>
          </PokemonPreView>
          </FlipCard>
        {/await}
      </a>
    </li>
  {/each}
</ul>

  <div>
    {@render children()}
  </div>
</div>

<style>
div {
  display: flex;
}
ul{
  display: flex;
  flex-wrap: wrap;
  list-style: none;
  padding:0;
  gap: 1em;
  justify-content: center;
}

*{
  text-decoration: none;
  color: black;
}
</style>
