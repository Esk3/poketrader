<script lang="ts">
const { children, data }: any = $props();
console.log(data.user);
</script>

<header>
<nav>
<ul>
{#each data.pages as page}
  <li>
    <a href="{page.href}">{page.name}</a>
  </li>
{/each}
<li class="split"></li>
{#if !data.user}
<li><a href="/register">Register</a></li>
<li><a href="/login">Login</a></li>
{:else}
<li>Coins: {data.user.coins}</li>
<li>Logged in as: {data.user.username}</li>
<li><a href="/logout">Logout</a></li>
{/if}
</ul>
</nav>

</header>
<main>
{@render children()}
</main>

<style>
ul {
display: flex;
padding: 0;
list-style: none;
gap: 1em;
}
li.split {
margin-left: auto;
}
:root {
  --main: red;
  --second: blue;
  --text: black;
}
</style>
