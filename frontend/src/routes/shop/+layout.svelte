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
  <FlipCard color="blue" width="130px" height="200px" delay={i*100}>
  <PokemonPreView {pokemon} >
    {item.cost} coins
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
}
*{
  text-decoration: none;
color: black;
}
</style>
