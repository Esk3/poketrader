<script lang="ts">
  import PokemonPreView from "$lib/components/PokemonPreView.svelte";

  const { data } = $props();
  console.log(data);
</script>

<div>
  <PokemonPreView pokemon={data.pokemon} />
  <form method="post" action="/shop/{data.pokemon.pokemonId}?/buy">
    <input type="hidden" name="pokemon-id" value={data.pokemon.pokemonId} />
    <input
      type="submit"
      value="Buy {data.pokemon.name} for {data.pokemon.cost}"
    />
  </form>
</div>

<style>
  div {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
</style>
