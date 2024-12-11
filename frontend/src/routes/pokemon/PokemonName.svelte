<script lang="ts">
  import PokemonSprite from "$lib/components/PokemonSprite.svelte";

  const { pokemon } = $props();
</script>

<h2>{pokemon.name}</h2>
<PokemonSprite url={pokemon.spriteUrl} name={pokemon.name} />
