#include <windows.h>
#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <iostream>
#include <fstream>
#include <string>
#include <vector>

using namespace std;
 
//sizeof(WORD) = 16 bits = 2 bytes
//sizeof(DWORD) = 32 bits = 4 bytes
 
struct wav_header_t
{
    char rId[4]; //"RIFF" = 0x46464952
    DWORD rLen; //28 [+ sizeof(wExtraFormatBytes) + wExtraFormatBytes] + sum(sizeof(chunk.id) + sizeof(chunk.size) + chunk.size)
    char wId[4]; //"WAVE" = 0x45564157
    char fId[4]; //"fmt " = 0x20746D66
    DWORD fLen; //16 [+ sizeof(wExtraFormatBytes) + wExtraFormatBytes]
    WORD wFormatTag; 
    WORD nChannels; // моно/стерео
    DWORD nSamplesPerSec; // частота дискретизации
    DWORD nAvgBytesPerSec; // средне байт за секунду 
    WORD nBlockAlign; // количество байт для сэмпла
    WORD wBitsPerSample; // амплитуда
    //[WORD wExtraFormatBytes;]
    //[Extra format bytes]
};
 
struct chunk_t
{
    char id[4]; //"data" = 0x61746164
    DWORD size;
    //Chunk data bytes
};
 
int main(void)
{
	vector<WORD> finalData1;
	vector<WORD> finalData2;
	vector<WORD> finalData3;
	vector<WORD> finalData4;

	ifstream in("finalData.txt");
	string line;
	while (getline(in, line))
	{
		finalData1.push_back(stoi(line));
		getline(in, line);
		finalData2.push_back(stoi(line));
		getline(in, line);
		finalData3.push_back(stoi(line));
		getline(in, line);
		finalData4.push_back(stoi(line));
	}

	in.close();

    WORD nChannels = 1; // количество каналов, 1 - моно, 2 - стерео
    DWORD nSamplesPerSec = 110; // частота дискретизации 
    WORD wBitsPerSample = 16; // амплитуда 

	// Количество отсчётов для данных из файла
	int samples_count = finalData1.size();
 
    chunk_t chunk;
    *(DWORD *)&chunk.id = 0x61746164;
    chunk.size = samples_count * wBitsPerSample / 8;
 
    wav_header_t header;
    *(DWORD *)&header.rId = 0x46464952;
    header.rLen = 36 + chunk.size;
    *(DWORD *)&header.wId = 0x45564157;
    *(DWORD *)&header.fId = 0x20746D66;
    header.fLen = 16;
    header.wFormatTag = 1;
    header.nChannels = nChannels;
    header.nSamplesPerSec = nSamplesPerSec;
    header.wBitsPerSample = wBitsPerSample;
    header.nBlockAlign = header.nChannels * header.wBitsPerSample / 8;
    header.nAvgBytesPerSec = header.nSamplesPerSec * header.nBlockAlign;
 
	char str[100];


	ofstream wavFileStream("buffer\\stream1.wav", ios::out | ios::trunc | ios::binary);

	wavFileStream.write((char*)&header, sizeof(header));
	wavFileStream.write((char*)&chunk, sizeof(chunk));

	// Запись данных из файла
	for (int i = 0; i < finalData1.size(); ++i)
	{
		wavFileStream.write((char*)&finalData1[i], sizeof(finalData1[i]));
	}

	wavFileStream.close();


	wavFileStream.open("buffer\\stream2.wav", ios::out | ios::trunc | ios::binary);

	wavFileStream.write((char*)&header, sizeof(header));
	wavFileStream.write((char*)&chunk, sizeof(chunk));

	// Запись данных из файла
	for (int i = 0; i < finalData1.size(); ++i)
	{
		wavFileStream.write((char*)&finalData2[i], sizeof(finalData2[i]));
	}

	wavFileStream.close();


	wavFileStream.open("buffer\\stream3.wav", ios::out | ios::trunc | ios::binary);

	wavFileStream.write((char*)&header, sizeof(header));
	wavFileStream.write((char*)&chunk, sizeof(chunk));

	// Запись данных из файла
	for (int i = 0; i < finalData1.size(); ++i)
	{
		wavFileStream.write((char*)&finalData3[i], sizeof(finalData3[i]));
	}

	wavFileStream.close();


	wavFileStream.open("buffer\\stream4.wav", ios::out | ios::trunc | ios::binary);

	wavFileStream.write((char*)&header, sizeof(header));
	wavFileStream.write((char*)&chunk, sizeof(chunk));

	// Запись данных из файла
	for (int i = 0; i < finalData1.size(); ++i)
	{
		wavFileStream.write((char*)&finalData4[i], sizeof(finalData4[i]));
	}

	wavFileStream.close();
 
   
    return 0;
}