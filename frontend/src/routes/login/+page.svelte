<script lang="ts">
  import type { ActionData } from "./$types";
  const { form }: { form: ActionData } = $props();
</script>

<h1>Login</h1>

{#if form?.err}<p class="error">Invalid login attempt</p>{/if}

<form method="post">
  <label for="username">Username:</label>
  <input type="text" name="username" placeholder="username" id="username" />
  <input type="submit" value="Submit" />
</form>

<style>
  p.error {
    background-color: red;
  }
</style>
