#include "CheckpointTimeLogger.h"

void CheckpointTimeLogger::ResetLogger()
{
	m_RTBC.clear();
	m_TRT = 0.0f;
}

void CheckpointTimeLogger::SaveCheckpointTime(float RTBC)
{
	//std::ofstream myfile;
	std::fstream wasd;
	//if (wasd.good())
	//{
	wasd.open("C:\\test\\TIME.txt", std::fstream::out | std::fstream::app);
	//}m
	//else
	//{
	//	wasd.open("C:\\test\\TIME.txt");
	//}
	if (wasd.is_open()) {
		wasd << RTBC << "\n";
		//wasd.write(std::to_string(RTBC).c_str(),100);
		//wasd.write((char*)&RTBC, sizeof(float));
		wasd.close();
	}
	else {
		std::cout << "Whoopsies" << std::endl;
	}

	m_RTBC.push_back(RTBC);

	m_TRT += RTBC;
}

float CheckpointTimeLogger::GetTotalTime()
{
	return m_TRT;
}

float CheckpointTimeLogger::GetCheckpointTime(int index)
{
	return m_RTBC[index];
}

int CheckpointTimeLogger::GetNumCheckpoints()
{
	return m_RTBC.size();
}
