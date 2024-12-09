<script lang="ts">
  const { pokemon } = $props();
</script>

<div>
  <p>{pokemon.name}</p>
  <img src={pokemon.spriteUrl} />
  <p>{pokemon.cost}</p>
</div>
