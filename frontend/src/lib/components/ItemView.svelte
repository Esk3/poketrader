<script lang="ts">
  interface ItemView {
    inventoryId: number;
    PokemonId: number;
    spriteUrl: string;
    pokemonurl: string;
  }
  const { item }: { item: ItemView } = $props();
</script>
