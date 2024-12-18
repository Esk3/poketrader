<script>
  import PokemonPreView from "$lib/components/PokemonPreView.svelte";
  const { children, data } = $props();
  console.log(data);
</script>

<div>

<ul>
{#each data.pokemon as item}
<li>
<a href="/shop/{item.pokemonId}">
{#await item.pokemon then pokemon}
<PokemonPreView {pokemon} >
{item.cost} coins
</PokemonPreView>
{/await}
</a>
</li>
{/each}
</ul>

{@render children()}
</div>

<style>
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
