<script>
  import PokemonNameList from "../pokemon/PokemonNameList.svelte";
  const { children, data } = $props();
</script>

<div>
<PokemonNameList pokemon={data.pokemon} />

{@render children()}
</div>

<style>
div {
display: flex;
}
</style>
