<script lang="ts">
  const { data } = $props();
</script>

<a href="/trades/create">Ny trade</a>
<ul>
  {#each data.trades as trade}
    <li>
      {trade.tradeId}
    </li>
  {/each}
</ul>
