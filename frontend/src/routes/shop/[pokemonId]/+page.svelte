<script lang="ts">
  import PokemonPreView from "$lib/components/PokemonPreView.svelte";

  const { data } = $props();
  console.log(data);
</script>

<div>
  <a href="/shop">Close</a>
  <PokemonPreView pokemon={data.pokemon} />
  <form method="post" action="/shop/{data.pokemon.pokemonId}?/buy">
    <input type="hidden" name="pokemon-id" value={data.pokemon.pokemonId} />
    <button type="submit"
      >Buy {data.pokemon.name} for
      <span class="money">{data.pokemon.cost}</span></button
    >
  </form>
</div>

<style>
  div {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
</style>
