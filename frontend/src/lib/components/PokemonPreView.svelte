<script lang="ts">
  interface PokemonView {
    name: string;
    pokemonId: number;
    spriteUrl: string;
  }
  const { pokemon, children }: { pokemon: PokemonView, children: any | undefined } = $props();
</script>

<div data-pokemon-id={pokemon.pokemonId}>
  <p>{pokemon.name}</p>
  <img src={pokemon.spriteUrl} alt="{pokemon.name} sprite" />
  {@render children?.()}
</div>

<style>
  div {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 1em;
    border: 1px solid black;
    border-radius: 1em;
    background-color: var(--main);
    outline: 1em double blue;
    outline-offset: -1em;
    &:hover {
      box-shadow: 0 3px 10px black inset;
    }
  }
</style>
