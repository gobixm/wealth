<script lang="ts">
  import Button, { Label } from "@smui/button";
  import DataTable, {
    Body,
    Cell,
    Head,
    Pagination,
    Row,
  } from "@smui/data-table";
  import IconButton from "@smui/icon-button";
  import { onMount } from "svelte";
  import { apiUrl } from "./api";

  interface SecuritySyncProgress {
    id: string;
    completed: boolean;
    count: number;
  }

  interface Security {
    id: string;
    name: string;
    modified: Date;
    deleted: boolean;
    term: number;
  }

  let progress: SecuritySyncProgress;
  let progressTimer: NodeJS.Timer;

  let currentPage = 0;
  const pageSize = 25;
  let securities: Security[] = [];

  onMount(async () => {
    await getSecurities();
  });

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

  const getSecurities = async () => {
    const respose = await fetch(
      `${apiUrl}/secs?offset=${currentPage * pageSize}&limit=${pageSize}`
    );

    securities = await respose.json();
  };

  const movePage = async (offset: number) => {
    currentPage += offset;
    if (currentPage < 0) currentPage = 0;
    await getSecurities();
  };
</script>

<div class="sec-admin">
  <Button on:click={sync}><Label>Sync All Securities</Label></Button>
  {#if progress && !progress.completed}
    <div>Progress: {progress.count}, completed: {progress.completed}</div>
  {/if}

  <DataTable class="sec-list"
    ><Head>
      <Row>
        <Cell>Name</Cell>
        <Cell>ID</Cell>
        <Cell>Deleted</Cell>
        <Cell>Modified</Cell>
        <Cell>Term</Cell>
      </Row>
    </Head>
    <Body>
      {#each securities as security (security.id)}
        <Row>
          <Cell>{security.name}</Cell>
          <Cell>{security.id}</Cell>
          <Cell>{security.deleted ? "deleted" : ""}</Cell>
          <Cell>{security.modified}</Cell>
          <Cell>{security.term}</Cell>
        </Row>
      {/each}
    </Body>

    <Pagination slot="paginate">
      <IconButton
        class="material-icons"
        action="prev-page"
        title="Prev page"
        on:click={async () => await movePage(-1)}
        disabled={currentPage === 0}>chevron_left</IconButton
      >
      <IconButton
        class="material-icons"
        action="next-page"
        title="Next page"
        on:click={async () => await movePage(1)}>chevron_right</IconButton
      >
    </Pagination>
  </DataTable>
</div>

<style lang="scss">
  .sec-list {
    display: flex;
    flex-flow: column nowrap;
  }

  .sec-admin {
    display: flex;
    flex-flow: column nowrap;
  }
</style>
