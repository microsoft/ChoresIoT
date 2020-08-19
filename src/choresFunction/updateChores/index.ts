import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import {
  BlobServiceClient,
  StorageSharedKeyCredential,
} from "@azure/storage-blob";
import { getBlob } from "../utils";
import * as dotenv from "dotenv";
dotenv.config();
const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  const chores = JSON.stringify(req.body);

  let blockBlobClient = getBlob();

  const uploadBlobResponse = await blockBlobClient.upload(
    chores,
    Buffer.byteLength(chores)
  );

  context.res = {
    body: "Chores Updated",
  };
};

export default httpTrigger;
