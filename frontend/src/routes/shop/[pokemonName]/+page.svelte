<script lang="ts">
  const { data } = $props();
</script>

<p>{data.pokemon.name}</p>
<form method="post" action="/shop/{data.pokemonName}?/buy">
  <input type="hidden" name="pokemon-id" value={data.pokemon.pokemonId} />
  <input
    type="submit"
    value="Buy {data.pokemon.name} for {data.pokemon.cost}"
  />
</form>
maybe a way to buy multiple
