<script lang="ts">
  const { pages, user }: any = $props();
  let ul: HTMLUListElement;
</script>

<nav>
  <ul bind:this={ul}>
    <li class="menu">
      <button
        class="menu"
        aria-label="menu button"
        onclick={() => {
          ul.classList.toggle("show");
        }}><img src="/hamburger-icon.svg" alt="menu icon" /></button
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
      <li class="user money">{user.coins}</li>
      <li>Logged in as: <span class="user"> {user.username}</span></li>
      <li class="user">
        <form action="/logout" method="post">
          <input class="logout-button" type="submit" value="Logout" />
        </form>
      </li>
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
      & button {
        color: black;
      }
    }

    :global(ul.show > li) {
      display: flex !important;
    }
    :global(ul.show) {
      flex-direction: column;
    }
  }
  button.menu,
  input.logout-button {
    background: none;
    border: medium none;
  }
  button.menu:hover {
    background-color: darkred;
  }
</style>
