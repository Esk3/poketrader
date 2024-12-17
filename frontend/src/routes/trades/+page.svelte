<script lang="ts">
  const { data } = $props();
</script>

<a href="/trades/create">Ny trade</a>
<ul>
  {#each data.trades as trade}
    <li>
      <a href="/trades/{trade.id}">
        <h2>Trade with: {trade.username2}</h2>
        <p>Started: {trade.startTimestamp}</p>
      </a>
    </li>
  {/each}
</ul>
