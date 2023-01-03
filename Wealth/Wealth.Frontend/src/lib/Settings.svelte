<script lang="ts">
  import Tab, { Label } from "@smui/tab";
  import TabBar from "@smui/tab-bar";
  import CurrenciesList from "./CurrenciesList.svelte";
  import SecuritiesSettings from "./SecuritiesSettings.svelte";

  let active = "Securities";
</script>

<TabBar tabs={["Securities", "Currencies", "About"]} let:tab bind:active>
  <Tab {tab}>
    <Label>{tab}</Label>
  </Tab>
</TabBar>

{#if active === "Securities"}
  <SecuritiesSettings />
{/if}
{#if active === "Currencies"}
  <CurrenciesList />
{/if}
{#if active === "About"}
  About
{/if}
