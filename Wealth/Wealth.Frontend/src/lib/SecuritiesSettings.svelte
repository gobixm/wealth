<script lang="ts">
  import Button, { Label } from "@smui/button";
  import { apiUrl } from "./api";

  interface SecuritySyncProgress {
    id: string;
    completed: boolean;
    count: number;
  }

  let progress: SecuritySyncProgress;
  let progressTimer: NodeJS.Timer;

  const sync = async () => {
    const response = await fetch(`${apiUrl}/secs/sync`, { method: "POST" });
    progress = await response.json();

    clearInterval(progressTimer);

    progressTimer = setInterval(() => getProgress(), 1_000);
  };

  const getProgress = async () => {
    const response = await fetch(`${apiUrl}/secs/sync-progress/${progress.id}`);
    progress = await response.json();

    if (progress.completed) {
      clearInterval(progressTimer);
    }
  };
</script>

<Button on:click={sync}><Label>Sync All Securities</Label></Button>

{#if progress && !progress.completed}
  <div>Progress: {progress.count}, completed: {progress.completed}</div>
{/if}
