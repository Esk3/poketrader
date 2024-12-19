<script lang="ts">
  const { pages, user }: any = $props();
  let ul: HTMLUListElement;
</script>

<nav>
  <ul bind:this={ul}>
    <li class="menu">
      <button
        onclick={() => {
          ul.classList.toggle("show");
        }}>toggle</button
      >
    </li>
    {#each pages as page}
      <li>
        <a href={page.href} onclick={() => ul.classList.remove("show")}
          >{page.name}</a
        >
      </li>
    {/each}

    <li class="split"></li>

    {#if !user}
      <li class="user"><a href="/register">Register</a></li>
      <li class="user"><a href="/login">Login</a></li>
    {:else}
      <li>Coins: <span class="user">{user.coins}</span></li>
      <li>Logged in as: <span class="user"> {user.username}</span></li>
      <li class="user"><a href="/logout">Logout</a></li>
    {/if}
  </ul>
</nav>

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

  nav > ul {
    background-color: var(--main);
    height: fit-content;
    & li {
      border: 1px solid black;
      display: flex;
      justify-content: center;
      align-items: center;
      &.menu {
        display: none;
      }
    }
    & a {
      padding: 1em;
    }
    & * {
      color: white;
      text-decoration: none;
    }
  }

  @media screen and (max-width: 1050px) {
    li:not(.user) {
      display: none;
    }
    li.menu {
      display: block !important;
    }

    :global(ul.show > li) {
      display: flex !important;
    }
    :global(ul.show) {
      flex-direction: column;
    }
  }
</style>
