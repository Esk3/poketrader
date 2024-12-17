<script lang="ts">
  interface ItemView {
    id: number;
    pokemonId: number;
    spriteUrl: string;
    pokemonurl: string;
    name: string;
  }
  const { item }: { item: ItemView } = $props();
</script>

<div data-item-id={item.id} data-pokemon-id={item.pokemonId}>
  <p>{item.name}</p>
  <img src={item.spriteUrl} alt="{item.name} sprite" />
</div>
