<script lang="ts">
  import Button from "@smui/button";

  let count: number = 0;
  const increment = () => {
    count += 1;
  };

  let mode = import.meta.env.WEALTH_API_URL;
</script>

<Button on:click={increment}>
  count is {count}
  mode is {mode}
</Button>
