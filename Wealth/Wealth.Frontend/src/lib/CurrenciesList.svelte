<script lang="ts">
  import DataTable, { Body, Cell, Head, Row } from "@smui/data-table";
  import { onMount } from "svelte";
  import type { Currency } from "../models";
  import { apiUrl } from "./api";

  let currencies: Currency[] = [];

  onMount(async () => {
    await getCurrencies();
  });

  const getCurrencies = async () => {
    const respose = await fetch(`${apiUrl}/currencies`);

    currencies = await respose.json();
  };
</script>

<DataTable class="currency-list"
  ><Head>
    <Row>
      <Cell>ID</Cell>
      <Cell>Ticker</Cell>
      <Cell>Name</Cell>
    </Row>
  </Head>
  <Body>
    {#each currencies as currency (currency.id)}
      <Row>
        <Cell>{currency.id}</Cell>
        <Cell>{currency.ticker}</Cell>
        <Cell>{currency.name}</Cell>
      </Row>
    {/each}
  </Body>
</DataTable>

<style lang="scss">
  .currency-list {
    display: flex;
    flex-flow: column nowrap;
  }
</style>
