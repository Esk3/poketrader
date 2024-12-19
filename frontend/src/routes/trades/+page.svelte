<script lang="ts">
  const { data } = $props();
</script>

<a href="/trades/create">Ny trade</a>
<hr />
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

<style>
  ul {
    display: flex;
    flex-direction: column;
    list-style: none;
    padding: 0;
    & li {
      border: 1px solid black;
    }
    & a {
      text-decoration: none;
      color: var(--text);
      margin: 1em;
    }
  }
</style>
