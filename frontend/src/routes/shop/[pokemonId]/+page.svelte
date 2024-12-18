<script lang="ts">
  import PokemonPreView from "$lib/components/PokemonPreView.svelte";

  const { data } = $props();
</script>

<PokemonPreView pokemon={data.pokemon} />
<form method="post" action="/shop/{data.pokemonName}?/buy">
  <input type="hidden" name="pokemon-id" value={data.pokemon.pokemonId} />
  <input
    type="submit"
    value="Buy {data.pokemon.name} for {data.pokemon.cost}"
  />
</form>
