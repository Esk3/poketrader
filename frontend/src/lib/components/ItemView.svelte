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
