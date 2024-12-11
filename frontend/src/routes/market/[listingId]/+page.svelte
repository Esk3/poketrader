<script>
  import BidsList from "./BidsList.svelte";
  const { data } = $props();
</script>

<BidsList bids={data.bids} />
<form method="post" action="?/bid">
  <input type="number" name="amount" placeholder="amount" required />
  <input type="Submit" value="Submit" />
</form>
