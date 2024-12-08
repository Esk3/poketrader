<script lang="ts">
  import PokemonNameList from "./PokemonNameList.svelte";
  import type { PageData } from "./$types";

  const { data }: { data: PageData } = $props();
</script>

<h1>Pokemon</h1>

<PokemonNameList pokemon={data.pokemon} />
