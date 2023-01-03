<script lang="ts">
  import SegmentedButton, { Label, Segment } from "@smui/segmented-button";
  import Autocomplete from "@smui-extra/autocomplete";
  import Select, { Option } from "@smui/select";
  import type { AddTransactionRequest, Currency, Security } from "../models";
  import { apiUrl } from "./api";
  import Textfield from "@smui/textfield";
  import { onMount } from "svelte";
  import Button from "@smui/button";
  let operations = ["Buy", "Sell"];
  let selectedOperation = "Buy";
  let selectedSecurity: Security;
  let selectedDate: string = "";
  let selectedLots = 0;
  let selectedPricePerLot = 0;
  let selectedFee = 0;
  let selectedCurrency: Currency;
  let currencies: Currency[] = [];

  const searchSecurities = async (name: string): Promise<Security[]> => {
    if (name === "") {
      return [];
    }

    const respose = await fetch(`${apiUrl}/secs?name=${name}&limit=${100}`);

    return await respose.json();
  };

  onMount(async () => {
    await getCurrencies();
  });

  const getCurrencies = async () => {
    const respose = await fetch(`${apiUrl}/currencies`);

    currencies = await respose.json();
  };

  const addTransaction = async () => {
    const request: AddTransactionRequest = {
      currencyId: selectedCurrency.id,
      date: new Date(selectedDate),
      fee: selectedFee,
      lots: selectedLots,
      OperationType: selectedOperation === "Buy" ? "Buy" : "Sell",
      pricePerLot: selectedPricePerLot,
      SecurityId: selectedSecurity.id,
    };

    await fetch(`${apiUrl}/transactions`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });
  };
</script>

<div class="add-form">
  <SegmentedButton
    segments={operations}
    let:segment
    singleSelect
    bind:selected={selectedOperation}
  >
    <Segment {segment}>
      <Label>{segment}</Label>
    </Segment>
  </SegmentedButton>

  <Autocomplete
    search={searchSecurities}
    bind:value={selectedSecurity}
    getOptionLabel={(option) => (option ? `${option.id} (${option.name})` : "")}
    showMenuWithNoInput={false}
    label="Security"
  />

  <Textfield bind:value={selectedLots} label="Lots" type="number" />
  <Textfield
    bind:value={selectedPricePerLot}
    label="Price Per Lot"
    type="number"
  />
  <Textfield bind:value={selectedFee} label="Fee" type="number" />
  <Textfield bind:value={selectedDate} label="Date" type="datetime-local" />
  <Select bind:value={selectedCurrency} label="Currency">
    {#each currencies as currency}
      <Option value={currency}>{currency.ticker}</Option>
    {/each}
  </Select>

  <Button on:click={addTransaction}>{selectedOperation}</Button>
</div>

<style lang="scss">
  .add-form {
    display: flex;
    flex-flow: column nowrap;
  }
</style>
