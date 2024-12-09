<script>
  import PokemonList from "./PokemonList.svelte";
  const { children, data } = $props();
</script>

<div>
<PokemonList pokemon={data.pokemon}/>

{@render children()}
</div>

<style>
div {
display: flex;
}
</style>
