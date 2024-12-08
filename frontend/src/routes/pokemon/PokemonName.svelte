<script lang="ts">
  const { pokemon } = $props();
  const spriteUrl = (id) =>
    `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`;
  const split = pokemon.url.split("/");
  const id = split[split.length - 2];
</script>

<h2>{pokemon.name}</h2>
<img src={spriteUrl(id)} />
