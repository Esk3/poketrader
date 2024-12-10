<script lang="ts">
  const {
    url,
    id,
    name,
  }: {
    url: string | undefined;
    id: number | undefined;
    name: string | undefined;
  } = $props();
  const src =
    url ||
    `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`;
  const alt = name ? `${name} sprite` : "Pokemon sprite";
  const size = 96;
</script>

<img {src} width={size} height={size} {alt} />
