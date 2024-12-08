<script lang="ts">
const {children, data} = $props();
</script>

<header>
<nav>
<ul>
{#each data.pages as page}
<li>
<a href="{page.href}">{page.name}</a>
</li>
</ul>
</nav>
{/each}

</header>
<main>
{@render children()}
</main>
