import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import {
  BlobServiceClient,
  StorageSharedKeyCredential,
  BlobDownloadResponseModel,
} from "@azure/storage-blob";
import { getBlob } from "../utils";
import * as dotenv from "dotenv";

dotenv.config();

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  let blockBlobClient = getBlob();

  const downloadBlockBlobResponse: BlobDownloadResponseModel = await blockBlobClient.download(
    0
  );

  let blobBody = await streamToString(
    downloadBlockBlobResponse.readableStreamBody!
  );

  context.res = {
    body: blobBody,
    headers: {
      'Content-Type': 'application/json'
    }
  };
};

// A helper method used to read a Node.js readable stream into string
async function streamToString(readableStream: NodeJS.ReadableStream) {
  return new Promise((resolve, reject) => {
    const chunks: string[] = [];
    readableStream.on("data", (data) => {
      chunks.push(data.toString());
    });
    readableStream.on("end", () => {
      resolve(chunks.join(""));
    });
    readableStream.on("error", reject);
  });
}

export default httpTrigger;
